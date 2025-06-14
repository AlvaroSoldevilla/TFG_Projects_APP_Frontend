﻿namespace TFG_Projects_APP_Frontend.Components.CreateModal;

public static class FormDialog
{
    /*Creates the page for the form and navigates to it*/
    public static async Task<T> ShowCreateObjectMenuAsync<T>(string title) where T : new()
    {
        var tcs = new TaskCompletionSource<T>();
        var page = new DynamicInputPage<T>(tcs, title);
        await Application.Current.MainPage.Navigation.PushModalAsync(page);
        return await tcs.Task;
    }
}
