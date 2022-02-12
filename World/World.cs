using System;
using System.IO;

using Minecraft.Tools.fNbt;

namespace Minecraft.World
{
    public class World
    {
        private readonly string Name;
        private readonly bool Vanilla;
        private readonly FileStream Stream;
        private readonly string LevelDat;


        // NBT start
        public byte AllowCommands;
        public WorldBorder BorderCenter;
        public int ClearWeatherTime;
        public BossEvent[] CustomBossEvents;
        public DataPackList DataPacks;
        public int DataVersion;
        public long DayTime;
        public byte Difficulty;
        public byte DifficultyLocked;
        public NbtCompound DimensionData;
        public GameRules GameRules;
        public WorldGeneratorSettings WorldGenSettings;
        public int GameType;
        public byte Hardcore;
        public byte Initialized;
        public long LastPlayed;
        public string LevelName;
        public byte Raining;
        public int RainTime;
        public float SpawnAngel;
        public int SpawnX;
        public int SpawnY;
        public int SpawnZ;
        public byte Thundering;
        public int ThunderTime;
        public long Time;
        public int Version;
        public MinecraftVersion McVersion;
        public Guid WanderingTraderId;
        public int WanderingTraderSpawnChance;
        public int WanderingTraderSpawnDelay;
        public byte WasModded;
        // NBT end

        internal World(string name, bool vanilla)
        {
            // https://minecraft.fandom.com/wiki/Java_Edition_level_format#level.dat_format
            Name = name;
            Vanilla = vanilla;
            LevelDat = Path.Combine(name, "level.dat");

            AllowCommands = 0;
            BorderCenter = new WorldBorder(0, 0, 0.2, 60000000, 5, 60000000, 0, 5, 15);
            ClearWeatherTime = 0;
            CustomBossEvents = Array.Empty<BossEvent>();
            DataPacks = new DataPackList();
            DataVersion = 2586;
            DayTime = 0;
            Difficulty = 2;
            DifficultyLocked = 0;
            DimensionData = new NbtCompound("DimensionData");
            GameRules = new GameRules();
            WorldGenSettings = new WorldGeneratorSettings(0, 1);
            GameType = 0;
            Hardcore = 0;
            Initialized = 0;
            LastPlayed = (long)(DateTime.UtcNow - DateTime.UnixEpoch).TotalMilliseconds;
            LevelName = GetName();
            Raining = 0;
            RainTime = 0;
            SpawnAngel = 0;
            SpawnX = 0;
            SpawnY = 64;
            SpawnZ = 0;
            Thundering = 0;
            ThunderTime = 0;
            Time = 0;
            Version = 19133;
            McVersion = new MinecraftVersion(2586, "1.16.5", 0);
            WanderingTraderId = Guid.Empty;
            WanderingTraderSpawnChance = 25;
            WanderingTraderSpawnDelay = 24000;
            WasModded = 0;

            if (!Directory.Exists(name)) Directory.CreateDirectory(name);
            bool generate = !Exists(name);
            if (generate) Console.WriteLine("Starting generating " + name.ToUpper());

            Stream = File.Open(LevelDat, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);

            if (generate)
            {
                CreateDir("advancements");
                CreateDir("data");
                CreateDir("datapacks");
                CreateDir("playerdata");
                CreateDir("poi");
                CreateDir("region");
                CreateDir("stats");

                WriteLevelDat();
            }
        }

        public string GetName()
        {
            return Name;
        }

        public bool IsVanilla()
        {
            return Vanilla;
        }

        public static bool Exists(string name)
        {
            return Directory.Exists(name) && File.Exists(Path.Combine(name, "level.dat"));
        }

        private void CreateDir(string name)
        {
            if (!Directory.Exists(Path.Combine(GetName(), name))) Directory.CreateDirectory(Path.Combine(GetName(), name));
        }

        private void WriteLevelDat()
        {
            Stream.SetLength(0);
            Stream.Flush();


            NbtFile file = new NbtFile();
            NbtCompound root = new NbtCompound("Data");

            NbtCompound datapacks = new NbtCompound("DataPacks");
            NbtList dp_disabled = new NbtList("Disabled", NbtTagType.String);
            NbtList dp_enabled = new NbtList("Enabled", NbtTagType.String) { new NbtString("vanilla") };
            datapacks.Add(dp_disabled);
            datapacks.Add(dp_enabled);

            NbtCompound version = new NbtCompound("Version")
            {
                new NbtInt("Id", McVersion.Id),
                new NbtString("Name", McVersion.Name),
                new NbtByte("Snapshot", McVersion.Snapshot)
            };

            NbtList serverBrands = new NbtList("ServerBrands", NbtTagType.String) { new NbtString("TheCrafting Server_Modern") };


            root.Add(new NbtByte("allowCommands", AllowCommands));
            root.Add(new NbtDouble("BorderCenterX", BorderCenter.X));
            root.Add(new NbtDouble("BorderCenterZ", BorderCenter.Z));
            root.Add(new NbtDouble("BorderDamagePerBlock", BorderCenter.DamagePerBlock));
            root.Add(new NbtDouble("BorderSize", BorderCenter.Size));
            root.Add(new NbtDouble("BorderSafeZone", BorderCenter.SafeZone));
            root.Add(new NbtDouble("BorderSizeLerpTarget", BorderCenter.SizeLerpTarget));
            root.Add(new NbtLong("BorderSizeLerpTime", BorderCenter.SizeLerpTime));
            root.Add(new NbtDouble("BorderWarningBlocks", BorderCenter.WarningBlocks));
            root.Add(new NbtDouble("BorderWarningTime", BorderCenter.WarningTime));
            root.Add(new NbtInt("clearWeatherTime", ClearWeatherTime));
            root.Add(new NbtCompound("CustomBossEvents"));
            root.Add(datapacks);
            root.Add(new NbtInt("DataVersion", DataVersion));
            root.Add(new NbtLong("DayTime", DayTime));
            root.Add(new NbtByte("Difficulty", Difficulty));
            root.Add(new NbtByte("DifficultyLocked", DifficultyLocked));
            root.Add(DimensionData);
            root.Add(new NbtCompound("DragonFight"));
            root.Add(new NbtCompound("GameRules"));
            root.Add(new NbtCompound("WorldGenSettings"));
            root.Add(new NbtInt("GameType", GameType));
            root.Add(new NbtByte("hardcore", Hardcore));
            root.Add(new NbtByte("initialized", Initialized));
            root.Add(new NbtLong("LastPlayed", LastPlayed));
            root.Add(new NbtString("LevelName", LevelName));
            root.Add(new NbtByte("raining", Raining));
            root.Add(new NbtInt("rainTime", RainTime));
            root.Add(new NbtList("ScheduledEvents", NbtTagType.String));
            root.Add(serverBrands);
            root.Add(new NbtFloat("SpawnAngel", SpawnAngel));
            root.Add(new NbtInt("SpawnX", SpawnX));
            root.Add(new NbtInt("SpawnY", SpawnY));
            root.Add(new NbtInt("SpawnZ", SpawnZ));
            root.Add(new NbtByte("thundering", Thundering));
            root.Add(new NbtInt("thunderTime", ThunderTime));
            root.Add(new NbtLong("Time", Time));
            root.Add(new NbtInt("version", Version));
            root.Add(version);
            root.Add(new NbtIntArray("WanderingTraderId", new int[4]));
            root.Add(new NbtInt("WanderingTraderSpawnChance", WanderingTraderSpawnChance));
            root.Add(new NbtInt("WanderingTraderSpawnDelay", WanderingTraderSpawnDelay));
            root.Add(new NbtByte("WasModded", WasModded));

            file.RootTag.Add(root);
            file.SaveToStream(Stream, NbtCompression.GZip);
        }
    }
}
