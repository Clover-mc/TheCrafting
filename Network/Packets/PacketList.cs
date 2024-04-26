namespace Minecraft.Network.Packets
{
    public enum PacketList : byte
    {
        // For version 1.4
        Keepalive = 0x00,
        LoginRequest = 0x01,
        Handshake = 0x02,
        ChatMessage = 0x03,
        TimeUpdate = 0x04,
        EntityEquipment = 0x05,
        SpawnPosition = 0x06,
        UseEntity = 0x07,
        UpdateHealth = 0x08,
        Respawn = 0x09,
        Player = 0x0A,
        PlayerPosition = 0x0B,
        PlayerLook = 0x0C,
        PlayerPositionAndLook = 0x0D,
        PlayerDigging = 0x0E,
        PlayerBlockReplacement = 0x0F,
        HeldItemChange = 0x10,
        UseBed = 0x11,
        Animation = 0x12,
        EntityAction = 0x13,
        SpawnNamedEntity = 0x14,
        // 0x15
        CollectItem = 0x16,
        SpawnObjectVehicle = 0x17,
        SpawnMob = 0x18,
        SpawnPainting = 0x19,
        SpawnExperienceOrb = 0x1A,
        // 0x1B
        EntityVelocity = 0x1C,
        DestroyEntity = 0x1D,
        Entity = 0x1E,
        EntityRelativeMove = 0x1F,
        EntityLook = 0x20,
        EntityLookAndRelativeMove = 0x21,
        EntityTeleport = 0x22,
        EntityHeadLook = 0x23,
        // 0x24 - 0x25
        EntityStatus = 0x26,
        AttachEntity = 0x27,
        EntityMetadata = 0x28,
        EntityEffect = 0x29,
        RemoveEntityEffect = 0x2A,
        SetExperience = 0x2B,
        // 0x2C - 0x32
        ChunkData = 0x33,
        MultiBlockChange = 0x34,
        BlockChange = 0x35,
        BlockAction = 0x36,
        BlockBreakAnimation = 0x37,
        MapChunkBulk = 0x38,
        // 0x39 - 0x3B
        Explosion = 0x3C,
        SoundOrParticleEffect = 0x3D,
        NamedSoundEffect = 0x3E,
        // 0x3F - 0x45
        ChangeGameState = 0x46,
        SpawnGlobalEntity = 0x47,
        // 0x48 - 0x63
        OpenWindow = 0x64,
        CloseWindow = 0x65,
        ClickWindow = 0x66,
        SetSlot = 0x67,
        SetWindowItems = 0x68,
        UpdateWindowProperty = 0x69,
        ConfirmTransaction = 0x6A,
        CreativeInventoryAction = 0x6B,
        EnchantItem = 0x6C,
        // 0x6D - 0x81
        UpdateSign = 0x82,
        ItemDate = 0x83,
        UpdateTileEntity = 0x84,
        // 0x85 - 0xC7
        IncrementStatistic = 0xC8,
        PlayerListItem = 0xC9,
        PlayerAbilities = 0xCA,
        TabComplete = 0xCB,
        ClientSettings = 0xCC,
        ClientStatuses = 0xCD,
        // 0xCE - 0xF9
        PluginMessage = 0xFA,
        // 0xFB
        EncryptionKeyResponse = 0xFC,
        EncryptionKeyRequest = 0xFD,
        ServerListPing = 0xFE,
        DisconnectKick = 0xFF
    }
}
