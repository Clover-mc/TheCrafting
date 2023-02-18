namespace Minecraft.Packets
{
    public enum PacketList : byte
    {
        // Source: https://wiki.vg/index.php?title=Protocol&oldid=1028
        KEEPALIVE = 0x00,
        LOGIN_REQUEST = 0x01,
        HANDSHAKE = 0x02,
        CHAT_MESSAGE = 0x03,
        TIME_UPDATE = 0x04,
        ENTITY_EQUIPMENT = 0x05,
        SPAWN_POSITION = 0x06,
        USE_ENTITY = 0x07,
        UPDATE_HEALTH = 0x08,
        RESPAWN = 0x09,
        PLAYER = 0x0A,
        PLAYER_POSITION = 0x0B,
        PLAYER_LOOK = 0x0C,
        PLAYER_POSITION_AND_LOOK = 0x0D,
        PLAYER_DIGGING = 0x0E,
        PLAYER_BLOCK_REPLACEMENT = 0x0F,
        HELD_ITEM_CHANGE = 0x10,
        USE_BED = 0x11,
        ANIMATION = 0x12,
        ENTITY_ACTION = 0x13,
        SPAWN_NAMED_ENTITY = 0x14,
        // 0x15
        COLLECT_ITEM = 0x16,
        SPAWN_OBJECT_VEHICLE = 0x17,
        SPAWN_MOB = 0x18,
        SPAWN_PAINTING = 0x19,
        SPAWN_EXPERIENCE_ORB = 0x1A,
        // 0x1B
        ENTITY_VELOCITY = 0x1C,
        DESTROY_ENTITY = 0x1D,
        ENTITY = 0x1E,
        ENTITY_RELATIVE_MOVE = 0x1F,
        ENTITY_LOOK = 0x20,
        ENTITY_LOOK_AND_RELATIVE_MOVE = 0x21,
        ENTITY_TELEPORT = 0x22,
        ENTITY_HEAD_LOOK = 0x23,
        // 0x24
        // 0x25
        ENTITY_STATUS = 0x26,
        ATTACH_ENTITY = 0x27,
        ENTITY_METADATA = 0x28,
        ENTITY_EFFECT = 0x29,
        REMOVE_ENTITY_EFFECT = 0x2A,
        SET_EXPERIENCE = 0x2B,
        // 0x2C
        // 0x2D
        // 0x2E
        // 0x2F
        // 0x30
        // 0x31
        // 0x32
        CHUNK_DATA = 0x33,
        MULTI_BLOCK_CHANGE = 0x34,
        BLOCK_CHANGE = 0x35,
        BLOCK_ACTION = 0x36,
        BLOCK_BREAK_ANIMATION = 0x37,
        MAP_CHUNK_BULK = 0x38,
        // 0x39
        // 0x3A
        // 0x3B
        EXPLOSION = 0x3C,
        SOUND_OR_PARTICLE_EFFECT = 0x3D,
        NAMED_SOUND_EFFECT = 0x3E,
        // 0x3F
        // 0x40
        // 0x41
        // 0x42
        // 0x43
        // 0x44
        // 0x45
        CHANGE_GAME_STATE = 0x46,
        SPAWN_GLOBAL_ENTITY = 0x47,
        // 0x48
        // 0x49
        // 0x4A
        // 0x4B
        // 0x4C
        // 0x4D
        // 0x4E
        // 0x4F
        // 0x50
        // 0x51
        // 0x52
        // 0x53
        // 0x54
        // 0x55
        // 0x56
        // 0x57
        // 0x58
        // 0x59
        // 0x5A
        // 0x5B
        // 0x5C
        // 0x5D
        // 0x5E
        // 0x5F
        // 0x60
        // 0x61
        // 0x62
        // 0x63
        OPEN_WINDOW = 0x64,
        CLOSE_WINDOW = 0x65,
        CLICK_WINDOW = 0x66,
        SET_SLOT = 0x67,
        SET_WINDOW_ITEMS = 0x68,
        UPDATE_WINDOW_PROPERTY = 0x69,
        CONFIRM_TRANSACTION = 0x6A,
        CREATIVE_INVENTORY_ACTION = 0x6B,
        ENCHANT_ITEM = 0x6C,
        // 0x6D
        // 0x6E
        // 0x6F
        // 0x70
        // 0x71
        // 0x72
        // 0x73
        // 0x74
        // 0x75
        // 0x76
        // 0x77
        // 0x78
        // 0x79
        // 0x7A
        // 0x7B
        // 0x7C
        // 0x7D
        // 0x7E
        // 0x7F
        // 0x80
        // 0x81
        UPDATE_SIGN = 0x82,
        ITEM_DATE = 0x83,
        UPDATE_TILE_ENTITY = 0x84,
        // 0x85
        // 0x86
        // 0x87
        // 0x88
        // 0x89
        // 0x8A
        // 0x8B
        // 0x8C
        // 0x8D
        // 0x8E
        // 0x8F
        // 0x90
        // 0x91
        // 0x92
        // 0x93
        // 0x94
        // 0x95
        // 0x96
        // 0x97
        // 0x98
        // 0x99
        // 0x9A
        // 0x9B
        // 0x9C
        // 0x9E
        // 0x9F
        // 0xA0
        // 0xA1
        // 0xA2
        // 0xA3
        // 0xA4
        // 0xA5
        // 0xA6
        // 0xA7
        // 0xA8
        // 0xA9
        // 0xAA
        // 0xAB
        // 0xAC
        // 0xAD
        // 0xAE
        // 0xAF
        // 0xB0
        // 0xB1
        // 0xB2
        // 0xB3
        // 0xB4
        // 0xB5
        // 0xB6
        // 0xB7
        // 0xB8
        // 0xB9
        // 0xBA
        // 0xBB
        // 0xBC
        // 0xBD
        // 0xBE
        // 0xBF
        // 0xC0
        // 0xC1
        // 0xC2
        // 0xC3
        // 0xC4
        // 0xC5
        // 0xC6
        // 0xC7
        INCREMENT_STATISTIC = 0xC8,
        PLAYER_LIST_ITEM = 0xC9,
        PLAYER_ABILITIES = 0xCA,
        TAB_COMPLETE = 0xCB,
        CLIENT_SETTINGS = 0xCC,
        CLIENT_STATUSES = 0xCD,
        // 0xCE
        // 0xCF
        // 0xD0
        // 0xD1
        // 0xD2
        // 0xD3
        // 0xD4
        // 0xD5
        // 0xD6
        // 0xD7
        // 0xD8
        // 0xD9
        // 0xDA
        // 0xDB
        // 0xDC
        // 0xDD
        // 0xDE
        // 0xDF
        // 0xE0
        // 0xE1
        // 0xE2
        // 0xE3
        // 0xE4
        // 0xE5
        // 0xE6
        // 0xE7
        // 0xE8
        // 0xE9
        // 0xEA
        // 0xEB
        // 0xEC
        // 0xED
        // 0xEE
        // 0xEF
        // 0xF0
        // 0xF1
        // 0xF2
        // 0xF3
        // 0xF4
        // 0xF5
        // 0xF6
        // 0xF7
        // 0xF8
        // 0xF9
        PLUGIN_MESSAGE = 0xFA,
        // 0xFB
        ENCRYPTION_KEY_RESPONSE = 0xFC,
        ENCRYPTION_KEY_REQUEST = 0xFD,
        SERVER_LIST_PING = 0xFE,
        DISCONNECT_KICK = 0xFF
    }
}
