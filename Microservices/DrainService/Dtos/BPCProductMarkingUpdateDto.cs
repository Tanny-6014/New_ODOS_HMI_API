namespace DrainService.Dtos
{
    public class BPCProductMarkingUpdateDto
    {
        public int intCABProductMarkID { get; set; }
        public int tntLayer { get; set; }
        public string chrGenerationStatus { get; set; }
        public bool bitAssemblyIndicator { get; set; }
        public bool bitMachineIndicator { get; set; }
    }
}
