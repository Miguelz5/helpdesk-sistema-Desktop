using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;

namespace HelpDeskDesktop
{
    public static class GeminiService
    {
        private static readonly string API_KEY = "AIzaSyDZHh_gOJWcFObAwdBZRs_UexKh7FCEJ-8";

        private static readonly string API_URL = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

        public static string TestarApiKey()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(10);

                    client.DefaultRequestHeaders.Add("X-goog-api-key", API_KEY);

                    var requestBody = new
                    {
                        contents = new[]
                        {
                            new
                            {
                                parts = new[]
                                {
                                    new
                                    {
                                        text = "Responda apenas com a palavra 'SUCESSO' se estiver funcionando."
                                    }
                                }
                            }
                        }
                    };

                    string json = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = client.PostAsync(API_URL, content).Result;
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    string resultado = $"=== TESTE DA API ===\n";
                    resultado += $"Modelo: gemini-2.0-flash\n";
                    resultado += $"Status: {response.StatusCode}\n";
                    resultado += $"Sucesso: {response.IsSuccessStatusCode}\n";

                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
                            if (jsonResponse.candidates != null && jsonResponse.candidates.Count > 0)
                            {
                                string resposta = jsonResponse.candidates[0].content.parts[0].text;
                                resultado += $"Resposta: {resposta}\n";
                                resultado += "✅ API FUNCIONANDO CORRETAMENTE!";
                            }
                            else
                            {
                                resultado += "❌ Estrutura da resposta inválida\n";
                                resultado += $"Resposta bruta: {responseString}";
                            }
                        }
                        catch (Exception jsonEx)
                        {
                            resultado += $"❌ Erro ao ler JSON: {jsonEx.Message}\n";
                            resultado += $"Resposta bruta: {responseString}";
                        }
                    }
                    else
                    {
                        resultado += $"❌ Erro: {responseString}";
                    }

                    return resultado;
                }
            }
            catch (Exception ex)
            {
                return $"❌ Erro de conexão: {ex.Message}";
            }
        }

        public static string ObterResposta(string perguntaUsuario)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(30);

                    client.DefaultRequestHeaders.Add("X-goog-api-key", API_KEY);

                    var requestBody = new
                    {
                        contents = new[]
                        {
                            new
                            {
                                parts = new[]
                                {
                                    new
                                    {
                                        text = $"Você é um assistente virtual de HelpDesk técnico. " +
                                               $"Responda em português de forma clara, objetiva e útil. " +
                                               $"Contexto: sistema de chamados técnicos. " +
                                               $"Pergunta do usuário: {perguntaUsuario}"
                                    }
                                }
                            }
                        }
                    };

                    string json = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = client.PostAsync(API_URL, content).Result;
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    Console.WriteLine($"=== RESPOSTA DA API ===");
                    Console.WriteLine($"Status: {response.StatusCode}");
                    Console.WriteLine($"Resposta: {responseString}");

                    if (response.IsSuccessStatusCode)
                    {
                        dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);

                        if (jsonResponse.candidates != null && jsonResponse.candidates.Count > 0)
                        {
                            string resposta = jsonResponse.candidates[0].content.parts[0].text;
                            return resposta.Trim();
                        }
                        else
                        {
                            return "Resposta da API vazia. " + ObterRespostaLocal(perguntaUsuario);
                        }
                    }
                    else
                    {
                        return $"❌ Erro na API ({response.StatusCode}). Modo local: " + ObterRespostaLocal(perguntaUsuario);
                    }
                }
            }
            catch (Exception ex)
            {
                return $"⚠️ Erro de conexão: {ex.Message}. Modo local: " + ObterRespostaLocal(perguntaUsuario);
            }
        }

        private static string ObterRespostaLocal(string pergunta)
        {
            string perguntaLower = pergunta.ToLower();

            if (perguntaLower.Contains("olá") || perguntaLower.Contains("oi") || perguntaLower.Contains("ola"))
                return "Olá! Sou seu assistente virtual. Como posso ajudar você hoje?";

            if (perguntaLower.Contains("chamado") || perguntaLower.Contains("abrir"))
                return "Para abrir um chamado, clique em 'Criar Chamado' no menu lateral. Preencha título, categoria e descrição detalhada do problema.";

            if (perguntaLower.Contains("problema") || perguntaLower.Contains("erro"))
                return "Descreva o problema em detalhes. Você pode abrir um chamado técnico se precisar de suporte especializado.";

            if (perguntaLower.Contains("senha") || perguntaLower.Contains("login"))
                return "Para alterar senha ou problemas de login, acesse 'Meu Perfil' no menu lateral.";

            if (perguntaLower.Contains("faq") || perguntaLower.Contains("perguntas"))
                return "Confira a seção 'FAQ' no menu lateral para ver perguntas frequentes.";

            return "Desculpe, não entendi completamente. Pode reformular? Estou aqui para ajudar com:\n• Abertura de chamados\n• Problemas técnicos\n• Dúvidas do sistema";
        }
    }
}