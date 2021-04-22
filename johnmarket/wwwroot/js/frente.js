const enderecoProduto = "https://localhost:5001/Produtos/Produto/"
const enderecoGerarVenda = "https://localhost:5001/Produtos/GerarVenda/"
let produto
let compra = []
let __totalVenda__ = 0.0

$("#posvenda").hide()

atualizarTotal()

function atualizarTotal() {
    $("#totalVenda").html(__totalVenda__)
}

function preencherFormulario({nome, categoria, fornecedor, precoDeVenda}) {
    $("#campoNome").val(nome)
    $("#campoCategoria").val(categoria.nome)
    $("#campoFornecedor").val(fornecedor.nome)
    $("#campoPreco").val(precoDeVenda)
}

function zerarFormulario() {
    $("#campoNome").val("")
    $("#campoCategoria").val("")
    $("#campoFornecedor").val("")
    $("#campoPreco").val("")
    $("#campoQuantidade").val("")
}

function adicionarNaTabela(prod, qtd) {
    const produtoTemp = {}
    Object.assign(produtoTemp, produto)
    
    const venda = { 
        produto: produtoTemp, 
        quantidade: Number(qtd),
        subtotal: Number((produtoTemp.precoDeVenda * qtd).toFixed(2))
    }

    __totalVenda__ += venda.subtotal
    atualizarTotal()

    compra.push(venda)

    $("#compras").append(`<tr>
        <td>${prod.id}</td>
        <td>${prod.nome}</td>
        <td>${qtd}</td>
        <td>${prod.precoDeVenda}</td>
        <td>${prod.medicao}</td>
        <td>${Number((prod.precoDeVenda * qtd).toFixed(2))}</td>
        <td><button class="btn btn-danger">Remover</button></td>
    </tr>`)
}

$("#produtoForm").on("submit", event => {
    event.preventDefault()
    const produtoParaTabela = produto
    const qtd = $("#campoQuantidade").val()

    adicionarNaTabela(produtoParaTabela, qtd)

    zerarFormulario()
})

$("#pesquisar").click(() => {
    const codProduto = $("#codProduto").val();
    const enderecoTemp = enderecoProduto + codProduto

    $.post(enderecoTemp, (data, status) => {
            produto = data

            let med

            switch (produto.medicao) {
                case 0:
                    med = "L"
                    break;
                case 0:
                    med = "K"
                    break;
                case 0:
                    med = "U"
                    break;
                default:
                    med = "U"
                    break;
            }

            produto.medicao = med

            preencherFormulario(produto)
    }).fail(() => {
        alert("Produto inválido!")
    })
})

$("#finalizarVendaBTN").click(() => {
    if (__totalVenda__ <= 0) {
        alert("Compra inválida, nenhum produto adicionado!")
        return
    }

    let _valorPago = $("#valorPago").val()

    if (!isNaN(_valorPago)) {
        _valorPago = parseFloat(_valorPago)
        if (_valorPago >= __totalVenda__) {
            const _troco = (_valorPago - __totalVenda__).toFixed(2)

            $("#posvenda").show()
            $("#prevenda").hide()
            $("#valorPago").prop("disabled", true)

            $("#troco").val(_troco)

            compra.forEach(elemento => {
                elemento.produto = elemento.produto.id
            })

            const _venda = {total: __totalVenda__, troco: Number(_troco), produtos: compra}

            $.ajax({
                type: "POST",
                url: enderecoGerarVenda,
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(_venda),
                success: function (data) {
                    console.log("Dados enviados com sucesso!")
                    console.log(data)
                }
            })
        } else {
            alert("Valor pago insuficiente!")
            return
        }
    } else {
        alert("Valor pago inválido!")
        return
    }
})

function restaurarModal() {
    $("#posvenda").hide()
    $("#prevenda").show()
    $("#valorPago").prop("disabled", false)
    $("#troco").val("")
    $("#valorPago").val("")
}

$("#fecharModal").click(restaurarModal)