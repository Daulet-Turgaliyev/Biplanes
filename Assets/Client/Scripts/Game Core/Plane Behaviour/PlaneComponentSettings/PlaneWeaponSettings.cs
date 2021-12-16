
    public readonly struct PlaneWeaponSettings
    {
        public float CoolDown { get; }
        public float BulletAcceleration { get; }

        public PlaneWeaponSettings(float coolDown, float bulletAcceleration)
        {
            CoolDown = coolDown;
            BulletAcceleration = bulletAcceleration;
        }
    }
