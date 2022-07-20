#!/bin/bash
PUBLISH_DIR=drawer/web/publish # publish directory
BLUE_DIR=drawer/web/blue # blue deployment directory
GREEN_DIR=drawer/web/green # green deployment directory

BLUE_NGINX_CONF=/etc/nginx/sites-available/drawer-web-blue.conf # blue nginx confi
GREEN_NGINX_CONF=/etc/nginx/sites-available/drawer-web-green.conf # green nginx config
UPSTREAM_NGINX_CONF=/etc/nginx/sites-enabled/drawer-web-upstream.conf # link of blue or green

BLUE_URL=localhost:11000 # blue server url
GREEN_URL=localhost:12000 # green server url
DEPLOY_COLOR_ROUTE=DevOps/DeploymentColor # route of deployment color

BLUE_SERVICE=drawer-web-blue.service # blue service name
GREEN_SERVICE=drawer-web-green.service # green service name