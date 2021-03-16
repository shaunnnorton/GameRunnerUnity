
from flask import Blueprint,jsonify ,request,redirect, url_for, send_from_directory
from models import Game, User



main = Blueprint('main', __name__)



@main.route('/')
def home_response():
    response = {
        "Response":"GOOD",
        "Data":None
    }
    
    return jsonify(response)

