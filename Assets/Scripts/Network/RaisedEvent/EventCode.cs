

public enum EventCode 
{
    noEvent = 0,
    onUpdateDeck = 1, // Lệnh cho mọi người phải update deck
    onUpdateHand = 2,
    onUpdateQueue = 3,
    onUpdateQuestion = 4, // Lệnh cho mọi người phải update question
    onUpdateBattle = 5,
    onUpdateDrop = 6,
    onLoadCardImg = 7, //Lúc bắt đầu Duel thì xem thử 2 bên đã load card id của nhau chưa
    onUpdateCheat = 8,
}
