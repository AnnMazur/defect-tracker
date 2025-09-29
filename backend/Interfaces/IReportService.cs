using System;
using System.Threading;
using System.Threading.Tasks;
using defectTracker.DTOs;

namespace defectTracker.Interfaces
{
    public interface IReportService
    {
        /// <summary>
        /// Возвращает CSV (или путь/поток) с агрегированной статистикой по фильтру.
        /// </summary>
        Task<byte[]> ExportDefectsCsvAsync(DefectFilterDto filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Формирование аналитических данных (например: количество дефектов по статусу/проекту).
        /// </summary>
        Task<object> GetAnalyticsAsync(DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default);
    }
}