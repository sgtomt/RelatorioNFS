# 📊 Gerador de Relatório NFS

Aplicação web desenvolvida em ASP.NET Core (Razor Pages) para leitura de arquivos XML de notas fiscais e geração automática de relatórios em Excel.

🔗 Acesse online: https://gerador-relatorio-nfs.onrender.com/

---

## 🚀 Visão Geral

O sistema permite que o usuário faça upload de múltiplos arquivos XML de NFS-e, processe os dados automaticamente e gere uma planilha Excel pronta para uso financeiro.

Ideal para eliminar tarefas manuais repetitivas e reduzir erros na consolidação de dados fiscais.

---

## 🖥️ Interface

### Tela inicial
Interface simples e direta, guiando o usuário em 3 passos:

- Selecionar os XMLs
- Processar os dados
- Baixar o relatório

### Upload e geração
- Suporte a múltiplos arquivos
- Drag & Drop
- Listagem dos arquivos carregados
- Geração com um clique

---

## ⚙️ Funcionalidades

- 📂 Upload de múltiplos XMLs (NFS-e / NF-e / CTe)
- 🔍 Leitura automática dos dados relevantes
- 📊 Consolidação das informações
- 📥 Geração de relatório em Excel
- ⚡ Processamento rápido direto no servidor
- 🌐 Interface web simples e intuitiva

---

## 🧠 Como funciona

1. O usuário acessa a aplicação
2. Envia um ou mais arquivos XML
3. O sistema:
   - Lê cada XML
   - Extrai dados como emissor, valores e datas
4. Um relatório consolidado é gerado automaticamente
5. O download é iniciado no navegador

---

## 🐳 Executando localmente (Docker)

### Clone o projeto

```bash
git clone https://github.com/sgtomt/Gerador-Relat-rio-NFS.git
cd Gerador-Relat-rio-NFS
