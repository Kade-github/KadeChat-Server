// Lets start off with commands?
// Here is an example of a clear command:
if (chat.Equals("/clear"))
{
if (user.Equals("Kade"))
{
  ChatMsgs = "[KadeChat Main Server Cleared!]";
}
else
{
ChatMsgs = ChatMsgs + "\n[Server]: No Permission! " + user;
}
}                        

// So how do they work?
// Its simple, just put the command to were the chat is processing
