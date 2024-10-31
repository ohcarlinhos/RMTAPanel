using System.Web;
using Shared.General;
using Shared.General.Pagination;

namespace RMTAPanel.Utils;

public class HandleUrlQueries(HttpClient httpClient)
{
    public Uri FromPagination(string path, PaginationQuery paginationQuery)
    {
        var uriBuilder = new UriBuilder(httpClient.BaseAddress + path);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        query["page"] = paginationQuery.Page.ToString();
        query["perPage"] = paginationQuery.PerPage.ToString();
        query["search"] = paginationQuery.Search;
        query["orderBy"] = paginationQuery.OrderBy;
        query["sort"] = paginationQuery.Sort;

        uriBuilder.Query = query.ToString();

        return uriBuilder.Uri;
    }
}