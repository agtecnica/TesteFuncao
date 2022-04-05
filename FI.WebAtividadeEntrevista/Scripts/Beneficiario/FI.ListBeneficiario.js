
$(document).ready(function () {

    ListarBeneficiarios();
    
})

function ListarBeneficiarios() {
    $('#gridBeneficiarios').jtable({
        //title: 'Beneficiários',
        paging: false, //Enable paging
        pageSize: 5, //Set page size (default: 10)
        sorting: false, //Enable sorting
        defaultSorting: 'Nome ASC', //Set default sorting
        actions: {
            listAction: urlBeneficiarioList,
        },
        fields: {
            CPF: {
                title: 'CPF',
                width: '35%'
            },
            Nome: {
                title: 'Nome',
                width: '50%'
            },
            Alterar: {
                title: '',
                display: function (data) {
                    return '<button id=btnAlterar type= "button" class="btn btn-sm btn-info" onclick="return alterarBeneficiario(' + data.record.Id + ',' + data.record.IdCliente + ',' + '\'' + data.record.Nome + '\', \'' + data.record.CPF + '\' );" > Alterar</button >';
                }
            },
            Excluir: {
                title: '',
                display: function (data) {
                    return '<button id=btnExcluir type= "button" class="btn btn-sm btn-info" onclick="return excluirBeneficiario(' + data.record.Id + ',' + data.record.IdCliente + ',' + '\'' + data.record.Nome + '\', \'' + data.record.CPF + '\');" > Excluir</button >';
                }
            }
        }
    });

    //Load student list from server
    if (document.getElementById("gridBeneficiarios"))
        $('#gridBeneficiarios').jtable('load');
}

function alterarBeneficiario(id, idcliente, nome, cpf) {

    $('#BeneficiarioId').val(id);
    $('#NomeBeneficiario').val(nome);
    $('#CPFBeneficiario').val(cpf).trigger('input');
    $('#IdCliente').val(idcliente);

    $('#btnSalvarbeneficiario').text("Salvar");
}

function salvarbeneficiario() {

    var id = $("#BeneficiarioId").val();
    var cpf = $("#CPFBeneficiario").val();
    var nome = $("#NomeBeneficiario").val();

    var urlPostBeneficiario = "";

    if (nome == '' || nome == undefined) {
        ModalDialog("Falha na validação", "Nome Inválido!");
        return false;
    }

    if (!isCpf(cpf) || cpf == '' || cpf == undefined) {
        ModalDialog("Falha na validação", "CPF Inválido!");
        return false;
    }

    if (id > 0) {
        urlPostBeneficiario = urlPostAlterarBenefeciario;
    } else {
        urlPostBeneficiario = urlPostIncluirBenefeciario;
    }

    $.ajax({
        url: urlPostBeneficiario,
        method: "POST",
        data: {
            "Id": id,
            "CPF": cpf,
            "Nome": nome,
            "IdCliente": $("#IdCliente").val()
        },
        error:
            function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
        success:
            function (r) {
                $("#formCadastroBeneficiario")[0].reset();
                $("#BeneficiarioId").val('');
                ModalDialog("Sucesso!", r.Result)
                ListarBeneficiarios();
            }
    });
}

function excluirBeneficiario(id, idcliente, nome, cpf) {

    $.ajax({
        url: urlExcluirBenefeciario,
        method: "DELETE",
        data: {
            "Id": id,
            "CPF": cpf,
            "Nome": nome,
            "IdCliente": idcliente
        },
        error:
            function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
        success:
            function (r) {
                $("#formCadastroBeneficiario")[0].reset();
                $("#BeneficiarioId").val('');
                ModalDialog("Sucesso!", r.Result)
                ListarBeneficiarios();
            }
    });
}

function fecharModalBeneficiario() {
    $("#BeneficiarioId").val('');
    $("#formCadastroBeneficiario")[0].reset();
}