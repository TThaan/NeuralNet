namespace NeuralNet_UI
{
    enum MediatorToken
    {
        //StartStopVM_OnNetInitializedOrChanged,
        StatusVM_OnTrainerStatusChanged,
        StartStopVM_OnTrainerStatusChanged,
        // NetVM_OnNetInitializedOrChanged,
        NetVM_OnNetInitialized,
        NetVM_OnNetChanged,
        StartStopVM_OnSampleSetInitChange,
        PredictVM_OnInputOrOutputChanged,
        PredictVM_CanExecutesChanged,
    }
}