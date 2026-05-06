using Anthropic;
using Google.GenAI;
using Microsoft.Extensions.AI;
using OpenAI.Chat;
using Primerchatbot;
using Primerchatbot.Chatbots;
using System.Text;

Utilidades.CargarVariableDeAmbiente();

var modeloOpenAI = "gpt-5.4-nano-2026-03-17";
var llaveOpenAI = Environment.GetEnvironmentVariable("OPENAI_KEY");
var clienteOpenAI = new OpenAI.Chat.ChatClient(modeloOpenAI, llaveOpenAI).AsIChatClient();

var modeloAnthropic = "claude-haiku-4-5";
string llaveAnthropic = Environment.GetEnvironmentVariable("ANTHROPIC_KEY")!;
IChatClient clienteAnthropic = new AnthropicClient(){ApiKey = llaveAnthropic}.AsIChatClient().AsBuilder().ConfigureOptions(x => x.ModelId = modeloAnthropic).Build();

var modeloGemini = "gemini-2.5-flash";
var llaveGemini = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
IChatClient clienteGemini = new Client(apiKey: llaveGemini).AsIChatClient().AsBuilder().ConfigureOptions(x => x.ModelId = modeloGemini).Build();

await Chatbot.Correr(clienteGemini);
