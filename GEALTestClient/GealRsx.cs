namespace GEALTest
{
    public enum StageEnum
    {
        Stage000 = 1,
        Stage001,
        Stage002,
        Stage003,
        Stage004,
        Stage005,
        Stage006_a,
        Stage006_b,
        Stage006_c,
        Stage006_d,
        Stage007,
        Stage008,
        Nothig,
    }
    public enum ButtonEnum
    {
        Base = 0x4000,  /* Widget Button */ 
        _00_NextBtn = Base + 0x0001,
        _01_NextBtn = Base + 0x0002,
        _02_NextBtn = Base + 0x0003,
        _03_NextBtn = Base + 0x000B,
        _04_NextBtn = Base + 0x0027,
        _05_NextBtn = Base + 0x0029,
        _06_NextBtn = Base + 0x002C,
        _07_NextBtn = Base + 0x0032,
        _08_NextBtn = Base + 0x0033,
    }
}