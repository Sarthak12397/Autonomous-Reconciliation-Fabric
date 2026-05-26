public enum ReconciliationJobType
{
    Scheduled,   // Triggered by Hangfire on a cron schedule
    Manual,      // Triggered by an operator via API
    EventDriven  // Triggered by an upstream event (e.g., payment settled)
}