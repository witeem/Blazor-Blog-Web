namespace BlazorServerApp.Data;
public static class DataChartService
{
    public static int[][] GetOrderChartData() => new int[][] { new[] { 0, 0 }, new[] { 1, 2 }, new[] { 2, 1 }, new[] { 3, 3 }, new[] { 4, 2 }, new[] { 5, 4 } };

    public static int[] GetProfitChartData() => new[] { 2, 6, 4, 2, 4 };

    public static int[] GetEarningsChartData() => new[] { 53, 31, 16 };

    public static int[][] GetRevenueReportChartData() => new int[][] { new[] { 100, 180, 300, 250, 100, 50, 200, 140, 80 }, new[] { -180, -100, -70, -250, -130, -100, -90, -120 } };

    public static int[] GetBudgetChartData() => new[] { 150, 260, 160, 200, 150, 100, 200, 140 };
}
