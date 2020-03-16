let totalPrice = 0;
const listPrices = [];
// console.log(":0");

const sel = document.getElementsByTagName('select');
const totalPriceTag = document.getElementById("total-price");
// console.log(sel);

const calcTotalPrice = (index) => {
    let num = +sel[index].options[sel[index].selectedIndex].getAttribute("price");
    listPrices[index] = num;
    console.log(num);
}

for (let index = 0; index < sel.length; index++) {
    sel[index].addEventListener("click", () => {
        let num = +sel[index].options[sel[index].selectedIndex].getAttribute("price");
        listPrices[index] = num;
        let totalPrice = listPrices.reduce((a, b) => a + b, 0);
        totalPriceTag.innerHTML = `Total Price: <strong>€ ${totalPrice.toFixed(2)}</strong>`;
        console.log(totalPrice);
    });
}


