using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();

        // Eventos para atualizar total automaticamente
        txt_quantidade.TextChanged += OnQuantidadeOuPrecoChanged;
        txt_preco.TextChanged += OnQuantidadeOuPrecoChanged;
    }

    // Atualiza o total automaticamente
    private void OnQuantidadeOuPrecoChanged(object sender, TextChangedEventArgs e)
    {
        // Substitui vírgula por ponto para decimal
        string precoText = txt_preco.Text?.Replace(',', '.') ?? "0";
        string quantidadeText = txt_quantidade.Text ?? "0";

        if (!int.TryParse(quantidadeText, out int quantidade))
            quantidade = 0;

        if (!decimal.TryParse(precoText, out decimal preco))
            preco = 0;

        decimal total = quantidade * preco;
        lbl_total.Text = $"Total: R$ {total:F2}";
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Valida campos
            if (string.IsNullOrWhiteSpace(txt_descricao.Text))
            {
                await DisplayAlert("Erro", "Digite a descrição do produto.", "OK");
                return;
            }

            if (!int.TryParse(txt_quantidade.Text, out int quantidade) || quantidade <= 0)
            {
                await DisplayAlert("Erro", "Digite uma quantidade válida.", "OK");
                return;
            }

            string precoText = txt_preco.Text?.Replace(',', '.') ?? "0";
            if (!decimal.TryParse(precoText, out decimal preco) || preco <= 0)
            {
                await DisplayAlert("Erro", "Digite um preço válido.", "OK");
                return;
            }

            // Cria produto
            Produto p = new Produto
            {
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(quantidade),
                Preco = Convert.ToDouble(preco)
            };

            // Salva no banco
            await App.Db.Insert(p);

            // Mensagem de sucesso
            await DisplayAlert("Sucesso!", "Produto adicionado!", "OK");

            // Volta para a tela de lista de compras
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}
