 🖥️ Sistema Help Desk - Desktop

Sistema de gerenciamento de chamados técnicos desenvolvido em C Windows Forms para desktop, com integração completa à plataforma web e recursos de inteligência artificial.

 📋 Sobre o Projeto

O Sistema Help Desk Desktop é uma aplicação Windows desenvolvida para otimizar o atendimento técnico em empresas, permitindo o gerenciamento completo de chamados, comunicação em tempo real e automação de processos através de IA.

 ✨ Funcionalidades

 🎯 Gestão de Chamados
- Abertura de chamados com categorização automática
- Controle de prioridades (Urgente, Alta, Média, Baixa)
- Acompanhamento em tempo real do status
- Atribuição para técnicos específicos
- Histórico completo de alterações

 💬 Sistema de Chat Integrado
- Comunicação em tempo real entre usuários e técnicos
- Interface similar ao WhatsApp para melhor usabilidade
- Histórico de conversas persistente
- Anexos no chat

 🤖 Inteligência Artificial
- Sugestões automáticas de respostas usando Gemini AI
- Categorização inteligente de chamados
- FAQ dinâmico baseado em histórico
- Análise de similaridade entre problemas

 📊 Recursos Administrativos
- Dashboard com métricas de desempenho
- Relatórios personalizáveis
- Gestão de usuários e permissões
- Base de conhecimento com FAQs

 🛠️ Tecnologias Utilizadas

- Frontend: Windows Forms (C)
- Backend: ASP.NET Web API
- Banco de Dados: SQL Server
- IA: Google Gemini AI API
- Comunicação: SignalR (tempo real)
- Relatórios: Chart.js

 🚀 Como Executar

 Pré-requisitos
- .NET Framework 4.8 ou superior
- SQL Server 2012+
- Conexão com internet (para IA e atualizações)

 Instalação
1. Clone o repositório:
```bash
git clone https://github.com/seu-usuario/helpdesk-desktop.git
```

2. Configure a connection string no `App.config`:
```xml
<connectionStrings>
    <add name="DefaultConnection" 
         connectionString="Server=localhost;Database=HelpDeskDB;Integrated Security=true;" />
</connectionStrings>
```

3. Configure a API Key do Gemini AI:
```xml
<appSettings>
    <add key="GeminiApiKey" value="sua-api-key-aqui" />
</appSettings>
```

4. Compile e execute o projeto.

 🔧 Configuração

 Variáveis de Ambiente
- `ApiBaseUrl`: URL da API web
- `GeminiApiKey`: Chave da API do Google Gemini
- `DatabaseConnection`: String de conexão com SQL Server

 Permissões
- Usuário Comum: Abrir e acompanhar chamados
- Técnico: Atender chamados, usar chat
- Administrador: Gestão completa do sistema

 📱 Integrações

 🔗 Plataforma Web
- Sincronização em tempo real com versão web
- Mesmo banco de dados compartilhado
- Notificações cruzadas entre plataformas

 ☁️ API Externa
- Google Gemini AI para automação
- Serviços de e-mail para notificações
- Armazenamento em nuvem para anexos

 🐛 Solução de Problemas

 Problemas Comuns
1. Erro de conexão com API
   - Verifique a URL base nas configurações
   - Confirme se o serviço web está online

2. Chat não carrega
   - Verifique a conexão SignalR
   - Confirme as credenciais do usuário

3. IA não responde
   - Valide a API Key do Gemini
   - Verifique o limite de requisições

 🤝 Contribuição

1. Faça o fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para detalhes.

 👥 Desenvolvedores

- Miguel da Silva Faria - [@miguelfaria](https://github.com/miguelfaria)
- Gustavo Araújo - [@gustavo](https://github.com/gustavo)


---

⭐️ Se este projeto te ajudou, deixe uma estrela no repositório!
