namespace Game.UI
{
    [System.Serializable]
    public class UIActionSequence : UIAction
    {
        public UIActionSequenceProcessor Processor;
        public UIActionSequencePart[] SequenceParts;

        public override void DoAction()
        {
            Processor.StartNewSequence(SequenceParts);
        }
    }
}