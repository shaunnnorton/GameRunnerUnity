from flask import Blueprint,request,jsonify, redirect, url_for
from api_app.models import User, Game

from api_app import app, db

main = Blueprint('main',__name__)



@main.route('/')
def home_response():
    response = {
        "Response":"GOOD",
        "Data":None
    }
    
    return jsonify(response)