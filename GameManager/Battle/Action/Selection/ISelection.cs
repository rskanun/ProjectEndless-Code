public interface ISelection
{
    public void OpenSelection(SelectionData selectionData);
    public void CloseSelection();
    public void ReopenSelection();
    public void UndoSelection();
}