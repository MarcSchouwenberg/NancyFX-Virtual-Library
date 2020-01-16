window.onload = function(){
	var headerOpts = [
	["/",
	 "Home"],
	["/books/",
	 "Book Index"],
	["/books/new",
	 "New Book"],
	["/members/",
	 "Member Index"],
	["/members/new",
	 "New Member"],
	["/loans/",
	 "All Loans (index)"],
	["/loans/new",
	 "New Loan"],
	]
	for (var i = 0; i < headerOpts.length; i++) {
		var opt = document.createElement("a");
		opt.href = headerOpts[i][0];
		opt.innerHTML = '<button class="headerOpt">'+headerOpts[i][1]+'</button>';
		document.getElementById("header").appendChild(opt);
	}
}