namespace SteamData.Utils {
  public interface ICheckDuplicateHandling {
    bool IsHandledBefore (SteamDataContext db);
  }
}