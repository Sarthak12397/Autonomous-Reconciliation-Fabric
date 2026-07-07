public class ReconciliationJob
{
        public Guid Id{get; set;}
        public string IdempotencyKey{get; set;}
        public Guid CorrelationId{get; set;}
        public Guid? CausationId {get; set;}
        public Guid? SupersedesJobId {get; set;}
        public int Revision {get; set; }
        public ReconciliationJobType JobType{get; set;}
       public ReconciliationWindow   Window{get;set;}  
       public DateTime  CreatedAt{get; set;}       
       public ReconciliationJobStatus Status{get; private set;}    
       public int TotalRecordsProcessed{get; private set;}     
       public int DiscrepanciesFound{get; private set;}
       public bool CircuitBreakerTripped{get; private set;}
       
    
}