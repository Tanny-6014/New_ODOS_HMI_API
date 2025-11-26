using Newtonsoft.Json;
using System.Collections.Generic;

public class ApiResponseDto
{
    [JsonProperty("code")]
    public int Code { get; set; }

    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("result")]
    public List<ProjectDetailDto> Result { get; set; }
}

public class ProjectDetailDto
{
    [JsonProperty("projectId")]
    public string ProjectId { get; set; }

    [JsonProperty("address")]
    public string Address { get; set; }

    [JsonProperty("gate")]
    public string Gate { get; set; }

    [JsonProperty("projectAddressCode")]
    public string projectAddressCode { get; set; }
}
