using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();

        lst_produtos.ItemsSource = lista;

        // Sempre que a coleção mudar, recalcula o total
        lista.CollectionChanged += (s, e) => AtualizarTotal();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        lista.Clear(); // evita duplicar se voltar para a tela
        List<Produto> tmp = await App.Db.GetAll();

        tmp.ForEach(i => lista.Add(i));

        AtualizarTotal();
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void ToolbarItem_Limpar_Clicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirmação",
                                          "Deseja realmente limpar todos os produtos?",
                                          "Sim", "Não");

        if (confirm)
        {
            try
            {
                // Apaga todos os registros do banco
                await App.Db.DeleteAll();

                // Limpa a lista exibida
                lista.Clear();

                // Atualiza o total no rodapé
                AtualizarTotal();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }

    private void AtualizarTotal()
    {
        if (lblTotalGeral != null)
        {
            double total = lista.Sum(p => p.Total);
            lblTotalGeral.Text = $"R$ {total:F2}";
        }
    }

}
