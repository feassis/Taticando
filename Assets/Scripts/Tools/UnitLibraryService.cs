using MVC.Model.Unit;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

public class UnitLibraryService
{
    private const string unitLibraryBundlePath = "config/unit/library";
    private const string unitLibraryBundleName = "Unit Library";

    private UnitLibrary library;

    public static async Task<UnitLibraryService> Create()
    {
        var unitLibraryService = new UnitLibraryService();

        await unitLibraryService.Setup();

        return unitLibraryService;
    }

    public async Task Setup()
    {
        var loadRequest = Addressables.LoadAssetAsync<UnitLibrary>(unitLibraryBundlePath);

        await loadRequest.Task;

        library = loadRequest.Result;
    }

    public List<UnitSetupEntry> GetUnitsDataOfTeam(MVC.Model.Combat.TeamEnum team)
    {
        return library.GetUnitsDataOfTeam(team);
    }
}
