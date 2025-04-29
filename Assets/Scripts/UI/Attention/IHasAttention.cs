public interface IHasAttention {
    event System.Action hasAttentionChanged;
    bool HasAttention();
    void UpdateAttention();
}