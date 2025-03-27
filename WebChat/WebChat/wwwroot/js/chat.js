document.addEventListener('alpine:init', () => {
    Alpine.data('chatData', () => ({
        message: "",
        messages: [],
        currentUserId: null,
        connection: null,
        init() {
            //tao connection
            this.currentUserId = this.$refs.UserIdEle.value;
            this.connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();
            //ket noi
            this.connection.start().then(() => {
                console.log("ket noi thanh cong");
            });


            //tao su kien nhan tin nhan
            this.connection.on("ReceiveMessage", (mesg) => {
                //them ti nhan vao cuoi danh sach
                this.messages.push(mesg);
            });

            //su kien khi thay doi nguoi nhan
            let userCheckBoxes = document.querySelectorAll("input[name=user-check]");
            userCheckBoxes.forEach(ele => {
                ele.addEventListener("change", async () => {
                    let receiverId = ele.value;
                    this.messages = [];
                    await this.loadMessage(receiverId, true);
                });
            });
            this.$refs.chatBox.addEventListener("keydown", (e) => {
                if (e.key === "Enter") {
                    this.sendMessage();
                }

            });
            

        },
        sendMessage() {
            //lay tin nhan
            let mesg = this.message;
            let receiverId = document.querySelector("input[name=user-check]:checked")?.value;
            if (!mesg) {
                alert("vui lòng nhập tin nhắn");
                return;
            }
            if (!receiverId) {
                alert("vui lòng Chọn người nhận tin nhắn");
                return;
            }
            this.connection.invoke("SendMessage", mesg, parseInt(receiverId))
                .then(() => {
                    //xoa tin nhan trong o nhap lieu

                    this.message = '';
                    this.$refs.chatBox.focus();
                    //sau khi gui tin nhan thanh cong thi cuon den cuoi danh sach tin nhan
                    let chatbox = document.querySelector("#chat-window");
                    setTimeout(() => {
                        chatbox.scrollTo(0, chatbox.scrollHeight);
                    }, 0);

                }).catch(err =>
                    console.error(err.toString())
                )
        },
        async loadMoreMessage() {
            let receiverId = document.querySelector("input[name=user-check]:checked")?.value;
            if (!receiverId) {
                return;
            }
            await this.loadMessage(receiverId);
        },
        async loadMessage(receiverId, isScrollToEnd = false) {

            //load lai tin nhan voi nguoi duoc chon
            let lastMessageId = this.messages.length > 0 ? this.messages[0].id : '';
            let res = await fetch(`/Home/chat/${receiverId}/${lastMessageId}`); //lay lich su tin nhan giua 2 nguoi nay
            let data = await res.json(); //chuyen du lieu ve dang json 
            this.messages = data.concat(this.messages); //them tin nhan vao danh sach tin nhan

            //scroll den cuoi danh sach tin nhan
            let chatbox = document.querySelector("#chat-window");
            setTimeout(() => {
                chatbox.scrollTo(0, chatbox.scrollHeight);
            }, 0);
        }
    }))
})
