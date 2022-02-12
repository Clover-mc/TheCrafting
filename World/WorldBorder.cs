namespace Minecraft.World
{
    public class WorldBorder
    {
        public double X;
        public double Z;
        public double DamagePerBlock;
        public double Size;
        public double SafeZone;

        public double SizeLerpTarget;
        public long SizeLerpTime;

        public double WarningBlocks;
        public long WarningTime;

        public WorldBorder(double x, double z)
        {
            X = x;
            Z = z;
            DamagePerBlock = 0.2;
            Size = 60000000;
            SafeZone = 5;
            SizeLerpTarget = 60000000;
            SizeLerpTime = 0;
            WarningBlocks = 5;
            WarningTime = 15;
        }

        public WorldBorder(double x = 0, double z = 0, double damagePerBlock = 0.2, double size = 60000000, double safeZone = 5,
            double sizeLerpTarget = 60000000, long sizeLerpTime = 0, double warningBlocks = 5, long warningTime = 15)
        {
            X = x;
            Z = z;
            DamagePerBlock = damagePerBlock;
            Size = size;
            SafeZone = safeZone;
            SizeLerpTarget = sizeLerpTarget;
            SizeLerpTime = sizeLerpTime;
            WarningBlocks = warningBlocks;
            WarningTime = warningTime;
        }
    }
}
