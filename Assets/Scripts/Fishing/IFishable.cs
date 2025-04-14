public interface IFishable
{
    void OnHooked();
    void OnCaught();
    void OnEscaped();
    bool CheckInput(int direction, bool shrink);
    float GetStamina();
    int[] GetFishState();

    int GetFishType();
}

