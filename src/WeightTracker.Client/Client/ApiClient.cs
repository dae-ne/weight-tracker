﻿using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using WeightTracker.Contracts;
using WeightTracker.Contracts.DTOs;
using WeightTracker.Contracts.QueryParams;
using WeightTracker.Contracts.Requests;

namespace WeightTracker.Client.Client;

internal sealed class ApiClient(HttpClient client) : IApiClient
{
    public async Task<WeightDataGroupDto> GetWeightDataAsync(
        GetWeightDataQueryParams queryParams,
        string accessToken,
        CancellationToken cancellationToken = default)
    {
        SetAuthorizationHeader(accessToken);
        var queryString = queryParams.BuildQueryString();
        var uri = $"{Routes.GetWeightData}?{queryString}";
        var response = await client.GetAsync(uri, cancellationToken);
        return await response.ReadContentAsAsync<WeightDataGroupDto>(cancellationToken);
    }

    public async Task AddWeightDataAsync(
        AddWeightDataRequest request,
        string accessToken,
        CancellationToken cancellationToken = default)
    {
        SetAuthorizationHeader(accessToken);
        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        await client.PostAsync(Routes.AddWeightData, content, cancellationToken);
    }

    public async Task UpdateWeightDataAsync(
        string date,
        UpdateWeightDataRequest request,
        string accessToken,
        CancellationToken cancellationToken = default)
    {
        SetAuthorizationHeader(accessToken);
        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var uri = Routes.UpdateWeightData.Replace("{date}", date);
        await client.PutAsync(uri, content, cancellationToken);
    }

    public async Task DeleteWeightDataAsync(
        string date,
        string accessToken,
        CancellationToken cancellationToken = default)
    {
        SetAuthorizationHeader(accessToken);
        var uri = Routes.DeleteWeightData.Replace("{date}", date);
        await client.DeleteAsync(uri, cancellationToken);
    }

    private void SetAuthorizationHeader(string accessToken)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }
}
