from flask import Blueprint,request,jsonify, redirect, url_for, render_template, send_file
import requests
from api_app.models import User, Game
from dotenv import load_dotenv
import os
from pprint import PrettyPrinter
load_dotenv()

from api_app import app, db, bcyrpt

main = Blueprint('main',__name__)



@main.route('/')
def home_response():
    
    return render_template("index.html")

@main.route("/DownloadOsx")
def download_mac():
    return send_file('static/GameCrusherMAC1.0.app.zip')

@main.route("/DownloadWin")
def download_windows():
    return send_file("static/GameCrusherWin1.0.zip")

@main.route('/API/Create/User',methods=['POST'])
def create_user():
    response_key = request.form.get('KEY')
    response_name = request.form.get('NAME')
    response_password = request.form.get("PASSWORD")
    if response_name and bcyrpt.check_password_hash(bcyrpt.generate_password_hash(os.getenv("SECRET_PASSCODE")),
        response_key):
        
        new_user = User(name=response_name,
            password=bcyrpt.generate_password_hash(response_password))
        db.session.add(new_user)
        db.session.commit()
        print(User.query.get(new_user.id))
        response = {
            "Response":"SUCCESS",
            "Data":[{'USER_ID':new_user.id},{"USER_PASSWORD":response_password}]
        }
        return jsonify(response)
    response = {
        "Response":"ERROR",
        "Data":["ERROR PROCESSING REQUEST CHECK KEY AND REQUIRED PARAMETERS"]
    }
    return jsonify(response)

# @main.route("/API/Add/Games", methods=['POST'])
# def append_user_games():
    
#     response_key = request.form.get('KEY')
#     response_name = request.form.get('NAME')
#     response_password = request.form.get("PASSWORD")
#     response_games = request.form.get("GAMES")
#     user = User.query.filter_by(name=response_name).first()
#     game_list = response_games.split(",")



#     if user and bcyrpt.check_password_hash(bcyrpt.generate_password_hash(os.getenv("SECRET_PASSCODE")),
#         response_key):
#         response = {
#             "Response":"SUCCESS",
#             "Data":[{"USER_GAMES":list()}]
#         }
#         for game in game_list:
#             url = 'https://api.rawg.io/api/games'
#             params = {
#                 'key':os.getenv('API_KEY'),
#                 'search':game,
#                 'page':1,
#                 'page_size':1
#             }
#             results = requests.get(url, params=params).json()
#             new_game = Game(title=game,box_art=results['results'][0]['background_image'])
#             user.games.append(new_game)
#             db.session.commit()
#         for game in user.games:
#             response['Data'][0]['USER_GAMES'].append(game.__repr__())

        
#         return jsonify(response)
    
    
#     response = {
#     "Response":"ERROR",
#     "Data":["ERROR PROCESSING REQUEST CHECK KEY AND REQUIRED PARAMETERS"]
#     }
#     return jsonify(response)


@main.route('/API/GET/USERIMAGES',methods=["POST"])
def get_game_image():
    response_key = request.form.get('KEY')
    response_name = request.form.get('NAME')
    response_password = request.form.get("PASSWORD")
    user = User.query.filter_by(name=response_name).first()
    
    if user and bcyrpt.check_password_hash(bcyrpt.generate_password_hash(os.getenv("SECRET_PASSCODE")),
        response_key):
        response = {
            "Response":"SUCCESS",
            "Data":[{"USER_GAMES":list()}]
        }
        for game in user.games:
            game_data = {
                'title':game.title,
                'image':game.box_art
            }
            
            response['Data'][0]['USER_GAMES'].append(game_data)
        return jsonify(response)

    response = {
        "Response":"ERROR",
        "Data":["ERROR PROCESSING REQUEST CHECK KEY AND REQUIRED PARAMETERS"]
    }
    return jsonify(response)


@main.route("/API/User", methods=["POST"])
def check_user():
    username = request.form.get("NAME")
    user = User.query.filter_by(name=username).first()
    if user:
        response = {
            "Response":"SUCCESS",
            "Data":f"USER EXISTS WITH USERNAME:{user.name}"
        }
        return jsonify(response)
    else:
        response = {
            "Response":"ERROR",
            "Data":f"USER DOES NOT EXIST WITH USERNAME:{username}"
        }
        return jsonify(response)

# @main.route('/API/Create/User',methods=['POST'])
# def create_user():
#     response_key = request.form.get('KEY')
#     response_name = request.form.get('NAME')
#     response_password = request.form.get("PASSWORD")
#     if response_name and bcyrpt.check_password_hash(bcyrpt.generate_password_hash(os.getenv("SECRET_PASSCODE")),
#         response_key):
        
#         new_user = User(name=response_name,
#             password=bcyrpt.generate_password_hash(response_password))
#         db.session.add(new_user)
#         db.session.commit()
#         print(User.query.get(new_user.id))
#         response = {
#             "Response":"SUCCESS",
#             "Data":[{'USER_ID':new_user.id},{"USER_PASSWORD":response_password}]
#         }
#         return jsonify(response)
#     response = {
#         "Response":"ERROR",
#         "Data":["ERROR PROCESSING REQUEST CHECK KEY AND REQUIRED PARAMETERS"]
#     }
#     return jsonify(response)
pp = PrettyPrinter(indent=4)
@main.route("/API/Add/Games", methods=['POST'])
def append_user_games():
    
    response_key = request.form.get('KEY')
    response_name = request.form.get('NAME')
    response_password = request.form.get("PASSWORD")
    response_games = request.form.get("GAMES")
    user = User.query.filter_by(name=response_name).first()
    game_list = response_games.split(",")



    if user and bcyrpt.check_password_hash(bcyrpt.generate_password_hash(os.getenv("SECRET_PASSCODE")),
        response_key):
        response = {
            "Response":"SUCCESS",
            "Data":[{"USER_GAMES":list()}]
        }
        for game in game_list:
            url = f'https://www.giantbomb.com/api/search/?api_key={os.getenv("NEW_API")}&query={game}&page=1&limit=1&format=json&resources=game'
            print(url)
            headers = {
                'user-agent':"SchoolGame"
            }
            results = requests.get(url,headers=headers).json()
            if results['number_of_page_results'] <= 0 :
                pass
            
            else:
                new_game = Game(title=game,box_art=results['results'][0]["image"]["original_url"])
                user.games.append(new_game)
                db.session.commit()
            

                #pp.pprint(results['results'][0]["image"]["original_url"])

           
        for game in user.games:
            response['Data'][0]['USER_GAMES'].append(game.__repr__())

        
        return jsonify(response)
    
    
    response = {
    "Response":"ERROR",
    "Data":["ERROR PROCESSING REQUEST CHECK KEY AND REQUIRED PARAMETERS"]
    }
    return jsonify(response)