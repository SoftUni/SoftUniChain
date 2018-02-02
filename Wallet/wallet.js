$(document).ready(function() {
    $('#linkHome').click(function() { showView("viewHome") });
    $('#linkCreateNewWallet').click(function() { showView("viewCreateNewWallet") });
    $('#linkOpenExistingWallet').click(function() { showView("viewOpenExistingWallet") });
    $('#linkAccountBalance').click(function() { showView("viewAccountBalance") });
    $('#linkSendTransaction').click(function() { showView("viewSendTransaction") });

    $('#buttonGenerateNewWallet').click(generateNewWallet);
    $('#buttonOpenExistingWallet').click(openExistingWallet);
    $('#buttonDisplayBalance').click(displayBalance);
    $('#buttonSignTransaction').click(signTransaction);
    $('#buttonSendSignedTransaction').click(sendSignedTransaction);
    $('#linkLogout').click(logout);
    
    // Attach AJAX "loading" event listener
    $(document).on({
        ajaxStart: function() { $("#loadingBox").show() },
        ajaxStop: function() { $("#loadingBox").hide() }    
    });
    
    function showView(viewName) {
        // Hide all views and show the selected view only
        $('main > section').hide();
        $('#' + viewName).show();
    }
    
    function showInfo(message) {
        $('#infoBox>p').html(message);
        $('#infoBox').show();
        $('#infoBox>header').click(function(){ $('#infoBox').hide(); });
    }

    function showError(errorMsg) {
        $('#errorBox>p').html("Error: " + errorMsg);
        $('#errorBox').show();
        $('#errorBox>header').click(function(){ $('#errorBox').hide(); });
    }
    
    function generateNewWallet() {
		let privateKey = '';
        for (let i = 0; i<64; i++)
			privateKey += Math.floor(Math.random() * 16).toString(16);
		let ec = new elliptic.ec('secp256k1')
		let keyPair = ec.genKeyPair();
		let privKey = keyPair.getPrivate().toString(16);
		let pubKey = keyPair.getPublic().getX().toString(16) + 
			(keyPair.getPublic().getY().isOdd() ? "1" : "0");
		let ripemd160 = new Hashes.RMD160;
		let address = ripemd160.hex(pubKey);
		
		$('#textareaCreateWalletResult').text(
			"Generated random private key: " + privKey + "\n" +
			"Extracted public key: " + pubKey + "\n" +
			"Extracted blockchain address: " + address
		);
    }
	
	function openExistingWallet() {
        // TODO
    }
	
	function displayBalance() {
        // TODO
    }
	
	function signTransaction() {
        // TODO
    }
	
	function sendSignedTransaction() {
        // TODO
    }

	function logout() {
        // TODO
    }
});