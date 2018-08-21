<?php
ini_set('display_errors', 3);

//Get the data from the two HTML documents
$postForm = require_once("GuestBook Post.html");
$viewForm = require_once("GuestBook View.html");

//Show both HTML documents
echo $postForm . "\n";
echo $viewForm . "\n";

?>