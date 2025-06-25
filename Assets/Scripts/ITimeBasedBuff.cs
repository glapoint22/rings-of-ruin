public interface ITimeBasedBuff
{
    float Duration { get; }
    PlayerState OnBuffExpired(PlayerState state);
}