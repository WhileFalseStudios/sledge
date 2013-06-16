namespace Sledge.Settings
{
    public enum HotkeysMediator
    {
        FourViewAutosize,
        FourViewFocusTopLeft,
        FourViewFocusTopRight,
        FourViewFocusBottomLeft,
        FourViewFocusBottomRight,

        FileNew,
        FileOpen,
        FileClose,
        FileSave,
        FileSaveAs,
        FileExport,
        FileCompile,

        HistoryUndo,
        HistoryRedo,

        OperationsCopy,
        OperationsCut,
        OperationsPaste,
        OperationsPasteSpecial,
        OperationsDelete,

        SelectAll,
        SelectionClear,

        ObjectProperties,

        Carve,
        MakeHollow,

        GroupingGroup,
        GroupingUngroup,
        GroupingToggleIgnore,

        TieToEntity,
        TieToWorld,

        SnapSelectionToGrid,
        SnapSelectionToGridIndividually,

        GridIncrease,
        GridDecrease,

        CenterAllViewsOnSelection,
        Center3DViewsOnSelection,
        Center2DViewsOnSelection,

        GoToCoordinates,
        GoToBrushID,
        ShowSelectedBrushID,

        ShowMapInformation,
        ShowEntityReport,

        QuickLoadPointfile,
        LoadPointfile,
        UnloadPointfile,
    }
}