using Anthropic;
using Google.GenAI;
using Microsoft.Extensions.AI;
using OpenAI.Chat;
using Primerchatbot;
using System.Text;

Utilidades.CargarVariableDeAmbiente();

//await ChatBotOpenAI.Correr();
//await ChatBotAnthropic.Correr();
await ChatBotGemini.Correr();
