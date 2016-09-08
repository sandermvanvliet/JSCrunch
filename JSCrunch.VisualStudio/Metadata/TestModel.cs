namespace JSCrunch.VisualStudio.Metadata
{
    public class TestModel
    {
        public TestModel Clone()
        {
            return new TestModel { Name = Name };
        }

        public string Name { get; set; }
    }
}