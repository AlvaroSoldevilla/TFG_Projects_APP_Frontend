namespace TFG_Projects_APP_Frontend.Components.CreateModal;

public static class FormDialog
{
    public static async Task<T> ShowCreateObjectMenuAsync<T>() where T : new()
    {
        var tcs = new TaskCompletionSource<T>();
        var page = new DynamicInputPage<T>(tcs);
        await Application.Current.MainPage.Navigation.PushModalAsync(page);
        return await tcs.Task;
    }
}
