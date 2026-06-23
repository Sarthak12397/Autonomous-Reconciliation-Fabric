public class ReconciliationJob
{
        public Guid Id{get; set;}
        public string IdempotencyKey{get; set;}
        public Guid CorrelationId{get; set;}
        public Guid? CausationId {get; set;}
        public Guid? SupersedesJobId {get; set;}
        public int Revision {get; set; }
        public ReconciliationJobType JobType{get; set;}
        public 


    
}