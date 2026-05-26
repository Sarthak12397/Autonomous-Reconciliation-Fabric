public enum DiscrepancySeverity
{
    Low,      // Amount diff < threshold, auto-resolvable
    Medium,   // Amount diff > threshold OR status conflict — needs strategy evaluation
    High,     // Missing records OR duplicate with financial impact
    Critical  // Large amount gap OR cross-currency OR unresolvable automatically
}