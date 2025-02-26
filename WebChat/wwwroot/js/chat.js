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
            this.connection.start();

            //tao su kien nhan tin nhan
            this.connection.on("ReceiveMessage", (mesg) => {
                //them ti nhan vao cuoi danh sach
                this.messages.push(mesg);
            });
            //su kien khi thay doi nguoi nhan
            let userCheckBoxes = document.querySelectorAll("input[name=user-check]");
            userCheckBoxes.forEach((ele) => {
                ele.addEventListener("change", async (e) => {
                    this.messages = [];
                    let receiverId = e.target.value;
                    //lay tin nhan
                    let response = await fetch(`/Home/Chat?id=${receiverId}`);
                    let data = await response.json();
                    data.forEach(mesg => {
                        this.messages.push(mesg);
                    });
                });
            });
       

        },
        sendMessage() {

            //lay tin nhan
            let mesg = this.message;
            let receiverId = document.querySelector("input[name=user-check]:checked")?.value;
            if (!mesg) {
                alert("Vui lòng nhập tin nhắn");
                return;
            }
            if (!receiverId) {
                alert("Vui lòng chọn người nhận");
                return;
            }
            this.connection.invoke("SendMessage", mesg, parseInt(receiverId))
                .then(() => {
                    //xoa tin nhan
                    this.message = "";
                }).catch(
                    err => console.error(err.toString()
                ));
        }
    }))
})