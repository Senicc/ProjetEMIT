using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ProjetEMIT.Models;
using ProjetEMIT.Repositories.Interfaces;
using ProjetEMIT.Services.Interfaces;

namespace ProjetEMIT.Services.Implementations;

public class RapportService : IRapportService
{
    private readonly IEmploiDuTempsService _emploiService;

    public RapportService(IEmploiDuTempsService emploiService, ISalleService _)
    {
        _emploiService = emploiService;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public Task<byte[]> GenerateRapportOccupationSallesAsync(DateOnly dateDebut, DateOnly dateFin)
    {
        throw new NotImplementedException();
    }

    public async Task<byte[]> GenerateEmploiDuTempsPdfAsync(int? classeId = null, int? enseignantId = null, int? salleId = null,
        DateOnly? dateDebut = null, DateOnly? dateFin = null)
    {
        IEnumerable<Seance> seances;

        if (classeId.HasValue)
            seances = await _emploiService.GetEmploiParClasseAsync(classeId.Value, dateDebut, dateFin);
        else if (enseignantId.HasValue)
            seances = await _emploiService.GetEmploiParEnseignantAsync(enseignantId.Value, dateDebut, dateFin);
        else if (salleId.HasValue)
            seances = await _emploiService.GetEmploiParSalleAsync(salleId.Value, dateDebut, dateFin);
        else
            seances = await _emploiService.GetAllSeancesAsync(dateDebut, dateFin);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(2, Unit.Centimetre);
                page.Header().Text("EMPLOI DU TEMPS - EMIT")
                    .FontSize(18).Bold().AlignCenter().FontColor(Colors.Blue.Darken2);

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        HeaderCell(header.Cell(), "Date");
                        HeaderCell(header.Cell(), "Horaire");
                        HeaderCell(header.Cell(), "Matiere");
                        HeaderCell(header.Cell(), "Enseignant");
                        HeaderCell(header.Cell(), "Classe");
                        HeaderCell(header.Cell(), "Salle");
                        HeaderCell(header.Cell(), "Type");
                    });

                    foreach (var seance in seances.OrderBy(s => s.Date).ThenBy(s => s.Creneau.HeureDebut))
                    {
                        BodyCell(table.Cell(), seance.Date.ToString("dd/MM/yyyy"));
                        BodyCell(table.Cell(), $"{seance.Creneau.HeureDebut} - {seance.Creneau.HeureFin}");
                        BodyCell(table.Cell(), seance.Matiere.Nom);
                        BodyCell(table.Cell(), $"{seance.Enseignant.Prenom} {seance.Enseignant.Nom}");
                        BodyCell(table.Cell(), seance.Classe.Nom);
                        BodyCell(table.Cell(), seance.Salle.NomSalle);
                        BodyCell(table.Cell(), seance.TypeSeance);
                    }
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Genere le ").FontSize(10);
                    text.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).FontSize(10);
                    text.Span(" - EMIT").FontSize(10);
                });
            });
        });

        return document.GeneratePdf();
    }

    private static void HeaderCell(IContainer cell, string label)
    {
        cell.Background(Colors.Blue.Darken2).Padding(8).AlignCenter()
            .Text(t => t.Span(label).FontColor(Colors.White).Bold());
    }

    private static void BodyCell(IContainer cell, string value)
    {
        cell.BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(8).AlignMiddle()
            .Text(value);
    }
}
