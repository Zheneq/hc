public interface IDataEntry
{
    int GetPrefabIndexToDisplay();
    void Setup(int displayIndex, _LargeScrollListItemEntry UIEntry);
#if EVOS
    string GetSortingKey();
#endif
}