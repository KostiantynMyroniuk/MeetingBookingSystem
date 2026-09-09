namespace MeetingBookingSystem.API.Common
{
    public record PaginatedList<T>(IEnumerable<T> Items, int PageNumber, int PageSize, int TotalCount);
}
