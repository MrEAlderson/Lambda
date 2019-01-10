﻿using System;
using System.Collections.Generic;
using TeeSharp.Common;
using TeeSharp.Common.Config;
using TeeSharp.Common.Console;
using TeeSharp.Common.Enums;
using TeeSharp.Common.Protocol;
using TeeSharp.Map.MapItems;
using TeeSharp.Server.Game.Entities;
using Pickup = TeeSharp.Common.Enums.Pickup;

namespace TeeSharp.Server.Game
{
    // TODO abstract
    public class GameController : BaseGameController
    {
        public override string GameType => "Test";

        public override void Init()
        {
            GameContext = Kernel.Get<BaseGameContext>();
            Server = Kernel.Get<BaseServer>();
            Console = Kernel.Get<BaseGameConsole>();
            Config = Kernel.Get<BaseConfig>();
            World = Kernel.Get<BaseGameWorld>();

            GameState = GameState.GameRunning;
            GameStateTimer = TimerInfinite;
            GameStartTick = Server.Tick;

            MatchCount = 0;
            RoundCount = 0;
            SuddenDeath = false;

            TeamScore = new int[2];
            SpawnPos = new IList<Vector2>[3]
            {
                new List<Vector2>(), // dm
                new List<Vector2>(), // red
                new List<Vector2>(), // blue
            };

            if (Config["SvWarmup"])
                SetGameState(GameState.WarmupUser, Config["SvWarmup"]);
            else
                SetGameState(GameState.WarmupGame, TimerInfinite);

            GameFlags = GameFlags.None;
            GameInfo = new GameInfo
            {
                MatchCurrent = MatchCount + 1,
                MatchNum = !string.IsNullOrEmpty(Config["SvMaprotation"]) && Config["SvMatchesPerMap"] != 0 
                    ? Config["SvMatchesPerMap"] 
                    : 0,
                ScoreLimit = Config["SvScorelimit"],
                TimeLimit = Config["SvTimelimit"],
            };

            GameContext.PlayerReady += OnPlayerReady;
            GameContext.PlayerEnter += OnPlayerEnter;
            GameContext.PlayerLeave += OnPlayerLeave;
        }

        private void OnPlayerLeave(BasePlayer player, string reason)
        {
            if (Server.ClientInGame(player.ClientId))
            {
                Console.Print(OutputLevel.Standard, "game", $"leave player={player.ClientId}:{Server.ClientName(player.ClientId)}");
            }

            player.CharacterSpawned -= OnCharacterSpawn;
            player.TeamChanged -= OnPlayerTeamChanged;
        }

        protected override void SetGameState(GameState state, int timer)
        {
            void CheckGameFlagsSurvival()
            {
                if (GameFlags.HasFlag(GameFlags.Survival))
                {
                    for (var i = 0; i < GameContext.Players.Length; i++)
                    {
                        if (GameContext.Players[i] != null)
                            GameContext.Players[i].RespawnDisabled = false;
                    }
                }
            }

            switch (state)
            {
                case GameState.WarmupGame:
                {
                    if (GameState != GameState.GameRunning && 
                        GameState != GameState.WarmupGame &&
                        GameState != GameState.WarmupUser)
                    {
                        return;
                    }

                    if (timer == TimerInfinite)
                    {
                        GameState = state;
                        GameStateTimer = timer;
                        CheckGameFlagsSurvival();
                    }
                    else if (timer == 0)
                    {
                        StartMatch();
                    }
                } break;

                case GameState.WarmupUser:
                {
                    if (GameState != GameState.GameRunning &&
                        GameState != GameState.WarmupUser)
                    {
                        return;
                    }

                    if (timer == 0)
                    {
                        StartMatch();
                        return;
                    }

                    if (timer < 0)
                    {
                        GameStateTimer = TimerInfinite;

                        if (Config["SvPlayerReadyMode"])
                            SetPlayersReadyState(false);
                    }
                    else
                    {
                        GameStateTimer = timer * Server.TickSpeed;
                    }

                    GameState = state;
                    CheckGameFlagsSurvival();
                } break;

                case GameState.StartCountdown:
                {
                    if (GameState != GameState.GameRunning &&
                        GameState != GameState.GamePaused &&
                        GameState != GameState.StartCountdown)
                    {
                        return;
                    }

                    GameState = state;
                    GameStateTimer = Server.TickSpeed * 3;
                    World.Paused = true;
                } break;

                case GameState.GameRunning:
                {
                    GameState = state;
                    GameStateTimer = TimerInfinite;
                    World.Paused = false;
                    SetPlayersReadyState(true);
                } break;

                case GameState.GamePaused:
                {
                    if (GameState != GameState.GameRunning &&
                        GameState != GameState.GamePaused)
                    {
                        return;
                    }

                    if (timer == 0)
                    {
                        SetGameState(GameState.StartCountdown, 0);
                        return;
                    }

                    if (timer < 0)
                    {
                        GameStateTimer = TimerInfinite;
                        SetPlayersReadyState(false);
                    }
                    else
                    {
                        GameStateTimer = timer * Server.TickSpeed;
                    }

                    GameState = state;
                    World.Paused = true;
                } break;

                case GameState.EndRound:
                case GameState.EndMatch:
                {
                    WincheckMatch();

                    if (GameState == GameState.EndMatch)
                        break;

                    if (GameState != GameState.GameRunning &&
                        GameState != GameState.EndRound ||
                        GameState != GameState.EndMatch ||
                        GameState != GameState.GamePaused)
                    {
                        return;
                    }

                    GameState = state;
                    GameStateTimer = timer * Server.TickSpeed;
                    SuddenDeath = false;
                    World.Paused = true;
                } break;
            }
        }

        protected override void WincheckMatch()
        {
            
        }

        protected override void SetPlayersReadyState(bool state)
        {
            throw new NotImplementedException();
        }

        protected override void StartMatch()
        {
            throw new NotImplementedException();
        }

        public override Team StartTeam()
        {
            return Team.Spectators;
        }

        public override bool IsTeamChangeAllowed(BasePlayer player)
        {
            return true;
        }

        public override bool IsTeamplay()
        {
            return GameFlags.HasFlag(GameFlags.Teams);
        }

        public override bool IsFriendlyFire(int clientId1, int clientId2)
        {
            return false;
        }

        public override bool CanSelfKill(BasePlayer player)
        {
            return true;
        }

        public override bool CanChangeTeam(BasePlayer player, Team team)
        {
            return true;
        }

        public override bool CanJoinTeam(BasePlayer player, Team team)
        {
            return true;
        }

        public override bool IsPlayerReadyMode()
        {
            return false;
        }

        public override bool GetRespawnDisabled(BasePlayer player)
        {
            if (GameFlags.HasFlag(GameFlags.Survival))
            {
                if (GameState == GameState.WarmupGame ||
                    GameState == GameState.WarmupUser ||
                    GameState == GameState.StartCountdown && GameStartTick == Server.Tick)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public override void Tick()
        {
            if (GameState != GameState.GameRunning)
            {
                if (GameStateTimer > 0)
                    GameStateTimer--;

                if (GameStateTimer == 0)
                {
                    
                }
                else
                {

                }
            }
        }

        public override int Score(int clientId)
        {
            return 0;
        }

        public override bool CanSpawn(Team team, int clientId, out Vector2 spawnPos)
        {
            if (team == Team.Spectators || World.Paused)
            {
                spawnPos = Vector2.Zero;
                return false;
            }

            var eval = new SpawnEval();
            if (IsTeamplay())
            {
                eval.FriendlyTeam = team;

                EvaluateSpawnType(eval, SpawnPos[1 + ((int)team & 1)]);
                if (!eval.Got)
                {
                    EvaluateSpawnType(eval, SpawnPos[0]);
                    if (!eval.Got)
                        EvaluateSpawnType(eval, SpawnPos[1 + (((int)team + 1) & 1)]);
                }
            }
            else
            {
                EvaluateSpawnType(eval, SpawnPos[0]);
                EvaluateSpawnType(eval, SpawnPos[1]);
                EvaluateSpawnType(eval, SpawnPos[2]);
            }

            spawnPos = eval.Position;
            return eval.Got;
        }

        protected virtual float EvaluateSpawnPos(SpawnEval eval, Vector2 pos)
        {
            var score = 0f;

            foreach (var character in Character.Entities)
            {
                var scoremod = 1f;
                if (eval.FriendlyTeam != Team.Spectators && character.Player.Team == eval.FriendlyTeam)
                    scoremod = 0.5f;

                var d = MathHelper.Distance(pos, character.Position);
                score += scoremod * (System.Math.Abs(d) < 0.00001 ? 1000000000.0f : 1.0f / d);
            }

            return score;
        }

        protected virtual void EvaluateSpawnType(SpawnEval eval, IList<Vector2> spawnPos)
        {
            for (var i = 0; i < spawnPos.Count; i++)
            {
                var positions = new[]
                {
                    new Vector2(0.0f, 0.0f),
                    new Vector2(-32.0f, 0.0f),
                    new Vector2(0.0f, -32.0f),
                    new Vector2(32.0f, 0.0f),
                    new Vector2(0.0f, 32.0f)
                };  // start, left, up, right, down

                var result = -1;
                for (var index = 0; index < 5 && result == -1; ++index)
                {
                    result = index;

                    foreach (var character in Character.Entities.Find(spawnPos[i], 64f))
                    {
                        if (GameContext.MapCollision.IsTileSolid(spawnPos[i] + positions[index]) ||
                            MathHelper.Distance(character.Position, spawnPos[i] + positions[index]) <=
                            character.ProximityRadius)
                        {
                            result = -1;
                            break;
                        }
                    }

                    if (result == -1)
                        continue;

                    var p = spawnPos[i] + positions[index];
                    var s = EvaluateSpawnPos(eval, p);

                    if (!eval.Got || eval.Score > s)
                    {
                        eval.Got = true;
                        eval.Score = s;
                        eval.Position = p;
                    }
                }
            }
        }

        public override void OnReset()
        {
            
        }

        public override void OnPlayerChat(BasePlayer player, GameMsg_ClSay message, out bool isSend)
        {
            isSend = true;
        }

        public override void OnPlayerInfoChange(BasePlayer player)
        {
            
        }

        protected override void OnPlayerReady(BasePlayer player)
        {
            player.CharacterSpawned += OnCharacterSpawn;
            player.TeamChanged += OnPlayerTeamChanged;
        }

        protected override void OnPlayerEnter(BasePlayer player)
        {
            player.Respawn();
            Console.Print(OutputLevel.Debug, "game",
                $"team_join player='{player.ClientId}:{Server.ClientName(player.ClientId)}' team={player.Team}");
            UpdateGameInfo(player.ClientId);
        }

        protected override void OnPlayerTeamChanged(BasePlayer player, Team prevteam, Team newteam)
        {
            Console.Print(OutputLevel.Debug, "game", $"team join player={player.ClientId}:{Server.ClientName(player.ClientId)} team={newteam}");

            if (newteam != Team.Spectators)
            {
                player.IsReadyToPlay = !IsPlayerReadyMode();
                if (GameFlags.HasFlag(GameFlags.Survival))
                    player.RespawnDisabled = GetRespawnDisabled(player);
            }
        }

        protected override void OnCharacterSpawn(BasePlayer player, Character character)
        {
            character.Died += OnCharacterDied;

            if (GameFlags.HasFlag(GameFlags.Survival))
            {
                character.IncreaseHealth(10);
                character.IncreaseArmor(5);

                character.GiveWeapon(Weapon.Gun, 10);
                character.GiveWeapon(Weapon.Grenade, 10);
                character.GiveWeapon(Weapon.Shotgun, 10);
                character.GiveWeapon(Weapon.Laser, 5);

                player.RespawnDisabled = GetRespawnDisabled(player);
            }
            else
            {
                character.IncreaseHealth(10);
                character.GiveWeapon(Weapon.Gun, 10);
            }
        }

        protected override void OnCharacterDied(Character victim, BasePlayer killer, Weapon weapon, ref int modespecial)
        {
            victim.Died -= OnCharacterDied;

            if (killer == null || weapon == BasePlayer.WeaponGame)
                return;

            // TODO

            if (weapon == BasePlayer.WeaponSelf)
                victim.Player.RespawnTick = Server.Tick + Server.TickSpeed * 3;

            if (GameFlags.HasFlag(GameFlags.Survival))
            {
                for (var i = 0; i < GameContext.Players.Length; i++)
                {
                    if (GameContext.Players[i] != null && GameContext.Players[i].DeadSpectatorMode)
                        GameContext.Players[i].UpdateDeadSpecMode();
                }
            }
        }

        public override void OnPlayerReadyChange(BasePlayer player)
        {
            if (Config["SvPlayerReadyMode"] && player.Team != Team.Spectators && !player.DeadSpectatorMode)
            {
                player.IsReadyToPlay ^= true;

                if (GameState == GameState.GameRunning && !player.IsReadyToPlay)
                    SetGameState(GameState.GamePaused, TimerInfinite);

                CheckReadyStates();
            }   
        }
        
        public override void OnEntity(Tile tile, Vector2 pos)
        {
            if (tile.Index < (int) MapEntities.EntityOffset)
                return;

            var entity = tile.Index - MapEntities.EntityOffset;
            Pickup? pickup = null;

            switch (entity)
            {
                case MapEntities.Spawn:
                    SpawnPos[0].Add(pos);
                    break;
                case MapEntities.SpawnRed:
                    SpawnPos[1].Add(pos);
                    break;
                case MapEntities.SpawnBlue:
                    SpawnPos[2].Add(pos);
                    break;
                case MapEntities.Armor:
                    pickup = Pickup.Armor;
                    break;
                case MapEntities.Health:
                    pickup = Pickup.Health;
                    break;
                case MapEntities.WeaponShotgun:
                    pickup = Pickup.Shotgun;
                    break;
                case MapEntities.WeaponGrenade:
                    pickup = Pickup.Grenade;
                    break;
                case MapEntities.PowerupNinja:
                    pickup = Pickup.Ninja;
                    break;
                case MapEntities.WeaponLaser:
                    pickup = Pickup.Laser;
                    break;
            }

            if (pickup.HasValue)
            {
                new Entities.Pickup(pickup.Value).Position = pos;
            }
        }

        protected override void CheckReadyStates()
        {
            // TODO
        }

        protected override void UpdateGameInfo(int clientId)
        {
            var msg = new GameMsg_SvGameInfo()
            {
                GameFlags = GameFlags,
                ScoreLimit = GameInfo.ScoreLimit,
                TimeLimit = GameInfo.TimeLimit,
                MatchCurrent = GameInfo.MatchCurrent,
                MatchNum = GameInfo.MatchNum,
            };

            if (clientId == -1)
            {
                for (var i = 0; i < GameContext.Players.Length; i++)
                {
                    if (GameContext.Players[i] == null || !Server.ClientInGame(i))
                        continue;

                    Server.SendPackMsg(msg, MsgFlags.Vital | MsgFlags.NoRecord, i);
                }
            }
            else
            {
                Server.SendPackMsg(msg, MsgFlags.Vital | MsgFlags.NoRecord, clientId);
            }
        }
        
        public override void OnSnapshot(int snappingId, out SnapshotGameData gameData)
        {
            gameData = Server.SnapshotItem<SnapshotGameData>(0);
            if (gameData == null)
                return;
            
            gameData.GameStartTick = 0; // GameStartTick
            gameData.GameStateFlags = GameStateFlags.None;
            gameData.GameStateEndTick = 0;
        }
    }
}