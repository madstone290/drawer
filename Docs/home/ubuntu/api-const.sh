#!/bin/bash
PUBLISH_DIR=/home/ubuntu/drawer/api/publish # publish directory
BLUE_DIR=/home/ubuntu/drawer/api/blue # blue deployment directory
GREEN_DIR=/home/ubuntu/drawer/api/green # green deployment directory

BLUE_NGINX_CONF=/etc/nginx/sites-available/drawer-api-blue.conf # blue nginx confi
GREEN_NGINX_CONF=/etc/nginx/sites-available/drawer-api-green.conf # green nginx config
UPSTREAM_NGINX_CONF=/etc/nginx/sites-enabled/drawer-api-upstream.conf # link of blue or green

BLUE_URL=localhost:11100 # blue server url
GREEN_URL=localhost:12100 # green server url
DEPLOY_COLOR_ROUTE=DevOps/DeploymentColor # route of deployment color

BLUE_SERVICE=drawer-api-blue.service # blue service name
GREEN_SERVICE=drawer-api-green.service # green service name